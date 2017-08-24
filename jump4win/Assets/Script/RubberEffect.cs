/***********************************************************************************************************
 * RUBBER EFFECT v2                                                                                        *
 * Changes: if the object is stopped, the vertices will sleep, saving CPU                                  *
 * You need to set the Vertex Colors of your 3d model, on your prefered 3d Modelling Tool                  *
 * by Rodrigo Pegorari - 2010 - http://rodrigopegorari.net                                                 *
 * based on the Processing 'Chain' code example (http://www.processing.org/learning/topics/chain.html)     *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;

public class RubberEffect : MonoBehaviour
{

	public RubberType Presets;

	public enum RubberType
	{
		Custom,
		RubberDuck,
		HardRubber,
		Jelly,
		SoftLatex
	}

	public float EffectIntensity = 1;
	public float gravity = 0;
	public float damping = 0.7f;
	public float mass = 1;
	public float stiffness = 0.2f;

	private Mesh WorkingMesh;
	private Mesh OriginalMesh;
	private float[] ColorIntensity;
	private VertexRubber[] vr;
	private Vector3[] V3_WorkingMesh;

	private bool sleeping = true;

	private Vector3 last_world_position;
	private Quaternion last_world_rotation;

	internal class VertexRubber
	{
		public int indexId;
		public float v_gravity;
		public float v_mass;
		public float v_stiffness;
		public float v_damping;
		public Vector3 pos;
		public Vector3 force;
		public Vector3 acc;
		public Vector3 last_pos, last_acc, last_force, last_vel;
		public bool v_sleeping = false;

		Vector3 vel = new Vector3();

		public VertexRubber(Vector3 v_target, float m, float g, float s, float d)
		{
			v_gravity = g;
			v_mass = m;
			v_stiffness = s;
			v_damping = d;
			pos = v_target;
		}

		public void update(Vector3 target)
		{

			if (!v_sleeping)
			{

				force.x = (target.x - pos.x) * v_stiffness;
				acc.x = force.x / v_mass;
				vel.x = v_damping * (vel.x + acc.x);
				pos.x += vel.x;

				force.y = (target.y - pos.y) * v_stiffness;
				force.y -= v_gravity / 10;
				acc.y = force.y / v_mass;
				vel.y = v_damping * (vel.y + acc.y);
				pos.y += vel.y;

				force.z = (target.z - pos.z) * v_stiffness;
				acc.z = force.z / v_mass;
				vel.z = v_damping * (vel.z + acc.z);
				pos.z += vel.z;

				if ((pos == last_pos) && (acc == last_acc) && (vel == last_vel) && (force == last_force)) v_sleeping = true;

				last_pos = pos;
				last_acc = acc;
				last_vel = vel;
				last_force = force;
			}

		}

	}

	void Start(){

		Debug.Log(Presets);
		MeshFilter filter = (MeshFilter)GetComponent(typeof(MeshFilter));
		OriginalMesh = Instantiate(filter.sharedMesh) as Mesh;

		WorkingMesh = Instantiate(filter.sharedMesh) as Mesh;
		filter.sharedMesh = WorkingMesh;

		ArrayList ActiveVertex = new ArrayList();

		for (int i = 0; i < WorkingMesh.vertices.Length; i++){
			Debug.Log (i);
			Debug.Log (OriginalMesh.colors.GetLength(0));
			if ((OriginalMesh.colors[i].r + OriginalMesh.colors[i].g + OriginalMesh.colors[i].b) != 3) ActiveVertex.Add(i);
		}

		ColorIntensity = new float[ActiveVertex.Count];
		vr = new VertexRubber[ActiveVertex.Count];

		for (int i = 0; i < ActiveVertex.Count; i++)
		{
			int ref_index = (int)ActiveVertex[i];
			ColorIntensity[i] = (1 - ((OriginalMesh.colors[ref_index].r + OriginalMesh.colors[ref_index].g + OriginalMesh.colors[ref_index].b) / 3)) * EffectIntensity;
			vr[i] = new VertexRubber(transform.TransformPoint(OriginalMesh.vertices[ref_index]), mass, gravity, stiffness, damping);
			vr[i].indexId = ref_index;
		}

		V3_WorkingMesh = OriginalMesh.vertices;

	}

	void Update()
	{

		checkPreset();

		if ((this.transform.position != last_world_position || this.transform.rotation != last_world_rotation) && sleeping)
		{
			for (int i = 0; i < vr.Length; i++)
			{
				vr[i].v_sleeping = false;
			}
			sleeping = false;
			Debug.Log("resetando");
		}

		if (!sleeping)
		{

			V3_WorkingMesh = OriginalMesh.vertices;
			int total = vr.Length;
			int v_sleeping_counter = 0;
			for (int i = 0; i < vr.Length; i++)
			{

				if (vr[i].v_sleeping)
				{
					v_sleeping_counter++;
				}

				Vector3 v3_target = transform.TransformPoint(V3_WorkingMesh[vr[i].indexId]);

				vr[i].v_gravity = gravity;
				vr[i].v_mass = mass;
				vr[i].v_stiffness = stiffness;
				vr[i].v_damping = damping;

				vr[i].update(v3_target);

				v3_target = transform.InverseTransformPoint(vr[i].pos);

				V3_WorkingMesh[vr[i].indexId] = Vector3.Lerp(V3_WorkingMesh[vr[i].indexId], v3_target, ColorIntensity[i] * EffectIntensity);

			}

			WorkingMesh.vertices = V3_WorkingMesh;
			WorkingMesh.RecalculateBounds();

			if (this.transform.position == last_world_position && this.transform.rotation == last_world_rotation && v_sleeping_counter == vr.Length)
			{
				sleeping = true;
			}
			else
			{
				last_world_position = this.transform.position;
				last_world_rotation = this.transform.rotation;
			}

		}

		if (sleeping)
		{
			WorkingMesh.vertices = V3_WorkingMesh;
			WorkingMesh.RecalculateBounds();
		}

	}

	void checkPreset() {

		switch (Presets)
		{
		case RubberType.HardRubber:
			gravity = 0f;
			mass = 8f;
			stiffness = 0.5f;
			damping = 0.9f;
			EffectIntensity = 0.5f;
			break;
		case RubberType.Jelly:
			gravity = 0f;
			mass = 1f;
			stiffness = 0.95f;
			damping = 0.95f;
			EffectIntensity = 1f;
			break;
		case RubberType.RubberDuck:
			gravity = 0f;
			mass = 2f;
			stiffness = 0.5f;
			damping = 0.85f;
			EffectIntensity = 1f;
			break;
		case RubberType.SoftLatex:
			gravity = 1f;
			mass = 0.9f;
			stiffness = 0.3f;
			damping = 0.25f;
			EffectIntensity = 1f;
			break;
		}

	}

}