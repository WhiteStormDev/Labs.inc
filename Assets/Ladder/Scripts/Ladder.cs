using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Ladder : MonoBehaviour {

	[Header("Шаблон:")]
	[SerializeField] private SpriteRenderer section;
	private Vector3 position;
	private float offset;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(string.Compare(other.tag, "Player") == 0)
		{
			LadderManager.SetLadderBounds(GetComponent<BoxCollider2D>().bounds);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(string.Compare(other.tag, "Player") == 0)
		{
			LadderManager.ResetStatus();
		}
	}

	SpriteRenderer GetLast()
	{
		Transform tr = (transform.childCount > 0) ? transform.GetChild(transform.childCount-1) : null;
		return (tr != null) ? tr.GetComponent<SpriteRenderer>() : null;
	}

	public void AddSection()
	{
		if(section == null) return;

		SpriteRenderer ren = GetLast();
		int id = transform.childCount + 1;

		if(ren != null)
		{
			position = ren.transform.position;
			offset = Vector3.Distance(ren.bounds.min, new Vector3(ren.bounds.min.x, ren.bounds.max.y, ren.bounds.min.z));
		}
		else
		{
			position = transform.position;
			offset = 0;
		}

		SpriteRenderer clone = Instantiate(section) as SpriteRenderer;
		clone.transform.SetParent(transform);
		clone.transform.name = "Section - " + id;
		clone.transform.position = position + Vector3.up * offset;

		CalculateTriggerSize(clone, id);
	}

	void CalculateTriggerSize(SpriteRenderer ren, int id)
	{
		if(ren == null)
		{
			ResetTriggerSize();
			return;
		}

		gameObject.layer = 2;
		BoxCollider2D box = GetComponent<BoxCollider2D>();
		box.isTrigger = true;
		float width = Vector3.Distance(ren.bounds.min, new Vector3(ren.bounds.max.x, ren.bounds.min.y, ren.bounds.min.z));
		float height = Height();
		float heightOffset = Vector3.Distance(ren.bounds.min, new Vector3(ren.bounds.min.x, ren.bounds.center.y, ren.bounds.min.z));
		box.size = new Vector2(width / 4, height);
		box.offset = (id > 1) ? new Vector2(0, height / 2 - heightOffset) : Vector2.zero;
	}

	float Height()
	{
		float value = 0;
		SpriteRenderer[] ren = GetComponentsInChildren<SpriteRenderer>();

		foreach(SpriteRenderer r in ren)
		{
			value += Vector3.Distance(r.bounds.min, new Vector3(r.bounds.min.x, r.bounds.max.y, r.bounds.min.z));
		}

		return value;
	}

	void ResetTriggerSize()
	{
		BoxCollider2D box = GetComponent<BoxCollider2D>();
		box.size = Vector2.one;
		box.offset = Vector2.zero;
	}

	public void RemoveSection()
	{
		SpriteRenderer last = GetLast();

		if(last != null)
		{
			#if UNITY_EDITOR
			DestroyImmediate(last.gameObject);
			#else
			Destroy(last.gameObject);
			#endif
		}

		CalculateTriggerSize(GetLast(), transform.childCount + 1);
	}

	public void ClearAll()
	{
		GameObject[] childs = new GameObject[transform.childCount];

		for(int i = 0; i < childs.Length; i++)
		{
			childs[i] = transform.GetChild(i).gameObject;
		}

		foreach(GameObject obj in childs)
		{
			#if UNITY_EDITOR
			DestroyImmediate(obj);
			#else
			Destroy(obj);
			#endif
		}

		ResetTriggerSize();
	}
}
