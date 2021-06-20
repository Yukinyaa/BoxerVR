using UnityEngine;
using Unity.Mathematics;

namespace SpaceGraphicsToolkit
{
	/// <summary>This component allows you to render the <b>SgtTerrain</b> component using the <b>SGT Planet</b> shader.</summary>
	[ExecuteInEditMode]
	[DefaultExecutionOrder(200)]
	[RequireComponent(typeof(SgtTerrain))]
	public class SgtTerrainPlanetMaterial : MonoBehaviour
	{
		/// <summary>The planet material that will be rendered.</summary>
		public Material Material { set { material = value; } get { return material; } } [SerializeField] private Material material;

		/// <summary>Normals bend incorrectly on high detail planets, so it's a good idea to fade them out. This allows you to set the camera distance at which the normals begin to fade out in local space.</summary>
		public double NormalFadeRange { set { normalFadeRange = value; } get { return normalFadeRange; } } [SerializeField] private double normalFadeRange;

		protected SgtTerrainPlanet cachedTerrain;

		private float bumpScale;

		private int bakedDetailTilingA;
		private int bakedDetailTilingB;
		private int bakedDetailTilingC;

		public void MarkAsDirty()
		{
			if (cachedTerrain != null)
			{
				cachedTerrain.MarkAsDirty();
			}
		}

		protected virtual void OnEnable()
		{
			cachedTerrain = GetComponent<SgtTerrainPlanet>();

			cachedTerrain.OnDrawQuad += HandleDrawQuad;
		}

		protected virtual void OnDisable()
		{
			cachedTerrain.OnDrawQuad -= HandleDrawQuad;

			MarkAsDirty();
		}

		protected virtual void Update()
		{
			if (normalFadeRange > 0.0 && SgtHelper.Enabled(cachedTerrain) == true)
			{
				var localPosition = cachedTerrain.GetObserverLocalPosition();
				var localAltitude = math.length(localPosition);
				var localHeight   = cachedTerrain.GetLocalHeight(localPosition);

				bumpScale = (float)math.saturate((localAltitude - localHeight) / normalFadeRange);
			}
			else
			{
				bumpScale = 1.0f;
			}

			bakedDetailTilingA = cachedTerrain.BakedDetailTilingA;
			bakedDetailTilingB = cachedTerrain.BakedDetailTilingB;
			bakedDetailTilingC = cachedTerrain.BakedDetailTilingC;
		}

		private void HandleDrawQuad(Camera camera, SgtTerrainQuad quad, Matrix4x4 matrix, int layer)
		{
			if (material != null)
			{
				var properties = quad.Properties;

				PreRenderMeshes(properties);

				foreach (var mesh in quad.CurrentMeshes)
				{
					Graphics.DrawMesh(mesh, matrix, material, gameObject.layer, camera, 0, properties);
				}
			}
		}

		protected virtual void PreRenderMeshes(SgtProperties properties)
		{
			properties.SetFloat(SgtShader._BumpScale, bumpScale);

			properties.SetInt(Shader.PropertyToID("_BakedDetailTilingA"), bakedDetailTilingA);
			properties.SetFloat(Shader.PropertyToID("_BakedDetailTilingAMul"), bakedDetailTilingA / 64.0f);
			properties.SetInt(Shader.PropertyToID("_BakedDetailTilingB"), bakedDetailTilingB);
			properties.SetInt(Shader.PropertyToID("_BakedDetailTilingC"), bakedDetailTilingC);
		}
	}
}

#if UNITY_EDITOR
namespace SpaceGraphicsToolkit
{
	using TARGET = SgtTerrainPlanetMaterial;

	[UnityEditor.CanEditMultipleObjects]
	[UnityEditor.CustomEditor(typeof(TARGET))]
	public class SgtTerrainPlanetMaterial_Editor : SgtEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			var markAsDirty = false;

			BeginError(Any(tgts, t => t.Material == null));
				Draw("material", "The planet material that will be rendered.");
			EndError();
			Draw("normalFadeRange", "Normals bend incorrectly on high detail planets, so it's a good idea to fade them out. This allows you to set the camera distance at which the normals begin to fade out in local space.");

			if (markAsDirty == true)
			{
				Each(tgts, t => t.MarkAsDirty());
			}
		}
	}
}
#endif