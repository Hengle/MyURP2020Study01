using UnityEngine;

namespace MyGraphics.Scripts.TAA
{
	public static class Vector2Extension
	{
		// positive if v2 is on the left side of v1
		public static float SignedAngle(this Vector2 v1, Vector2 v2)
		{
			Vector2 n1 = v1.normalized;
			Vector2 n2 = v2.normalized;

			float dot = Vector2.Dot(n1, n2);
			if (dot > 1.0f)
				dot = 1.0f;
			if (dot < -1.0f)
				dot = -1.0f;

			float theta = Mathf.Acos(dot);
			float sgn = Vector2.Dot(new Vector2(-n1.y, n1.x), n2);
			if (sgn >= 0f)
				return theta;
			else
				return -theta;
		}

		public static Vector2 Rotate(this Vector2 v, float theta)
		{
			float cs = Mathf.Cos(theta);
			float sn = Mathf.Sin(theta);
			float x1 = v.x * cs - v.y * sn;
			float y1 = v.x * sn + v.y * cs;
			return new Vector2(x1, y1);
		}
	}


	public static class Vector3Extension
	{
		public static Vector3 WithX(this Vector3 v, float x)
		{
			return new Vector3(x, v.y, v.z);
		}

		public static Vector3 WithY(this Vector3 v, float y)
		{
			return new Vector3(v.x, y, v.z);
		}

		public static Vector3 WithZ(this Vector3 v, float z)
		{
			return new Vector3(v.x, v.y, z);
		}
	}
}


public static class Matrix4x4Extension
{
	//https://stackoverflow.com/questions/53684676/behaviour-of-perspective-projection-to-convert-3d-point-to-screen
	public static Matrix4x4 GetPerspectiveProjection(float left, float right, float bottom, float top, float near,
		float far)
	{
		float x = (2.0f * near) / (right - left);
		float y = (2.0f * near) / (top - bottom);
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0f * far * near) / (far - near);
		float e = -1.0f;

		Matrix4x4 m = new Matrix4x4
		{
			[0, 0] = x,
			[0, 1] = 0,
			[0, 2] = a,
			[0, 3] = 0,
			[1, 0] = 0,
			[1, 1] = y,
			[1, 2] = b,
			[1, 3] = 0,
			[2, 0] = 0,
			[2, 1] = 0,
			[2, 2] = c,
			[2, 3] = d,
			[3, 0] = 0,
			[3, 1] = 0,
			[3, 2] = e,
			[3, 3] = 0
		};
		return m;
	}
}

public static class CameraExtension
{
	public static Matrix4x4 GetPerspectiveProjection(this Camera camera)
	{
		return GetPerspectiveProjection(camera, 0f, 0f);
	}

	public static Matrix4x4 GetPerspectiveProjection(this Camera camera, float texelOffsetX, float texelOffsetY)
	{
		if (camera == null)
			return Matrix4x4.identity;

		float oneExtentY = Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView);
		float oneExtentX = oneExtentY * camera.aspect;
		float texelSizeX = oneExtentX / (0.5f * camera.pixelWidth);
		float texelSizeY = oneExtentY / (0.5f * camera.pixelHeight);
		float oneJitterX = texelSizeX * texelOffsetX;
		float oneJitterY = texelSizeY * texelOffsetY;

		float cf = camera.farClipPlane;
		float cn = camera.nearClipPlane;
		float xm = (oneJitterX - oneExtentX) * cn;
		float xp = (oneJitterX + oneExtentX) * cn;
		float ym = (oneJitterY - oneExtentY) * cn;
		float yp = (oneJitterY + oneExtentY) * cn;

		return Matrix4x4Extension.GetPerspectiveProjection(xm, xp, ym, yp, cn, cf);
	}

	public static Matrix4x4 GetPerspectiveProjectionQuick(this Camera camera)
	{
		if (camera == null)
			return Matrix4x4.identity;

		float tanHalfFov = Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView);
		float aspect = camera.aspect;
		float far = camera.farClipPlane;
		float near = camera.nearClipPlane;

		Matrix4x4 mat = Matrix4x4.zero;

		mat[0, 0] = 1.0f / (aspect * tanHalfFov);
		mat[1, 1] = 1.0f / (tanHalfFov);
		mat[2, 2] = -(far + near) / (far - near);
		mat[2, 3] = -(2.0f * far * near) / (far - near);
		mat[3, 2] = -1;

		return mat;
	}

	public static Vector4 GetPerspectiveProjectionCornerRay(this Camera camera)
	{
		return GetPerspectiveProjectionCornerRay(camera, 0f, 0f);
	}

	public static Vector4 GetPerspectiveProjectionCornerRay(this Camera camera, float texelOffsetX, float texelOffsetY)
	{
		if (camera == null)
			return Vector4.zero;

		float oneExtentY = Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView);
		float oneExtentX = oneExtentY * camera.aspect;
		float texelSizeX = oneExtentX / (0.5f * camera.pixelWidth);
		float texelSizeY = oneExtentY / (0.5f * camera.pixelHeight);
		float oneJitterX = texelSizeX * texelOffsetX;
		float oneJitterY = texelSizeY * texelOffsetY;

		return new Vector4(oneExtentX, oneExtentY, oneJitterX, oneJitterY);
	}
}