using UnityEngine;

public static class Deadzone
{
	private static float defaultDeadzone = 0.001f;
	
	public static bool InReach(Vector3 a, Vector3 b)
	{
		float distance = Vector3.Distance(a, b);

		return distance < defaultDeadzone;
	}

    public static bool InReach(Vector3 a, Vector3 b, float deadZone)
    {
	    float distance = Vector3.Distance(a, b);

	    return distance < deadZone;
    }

	public static bool InReach(Vector4 a, Vector4 b)
	{
		float distance = Vector4.Distance(a, b);

		return distance < defaultDeadzone;
	}

	public static bool InReach(Vector4 a, Vector4 b, float deadZone)
	{
		float distance = Vector4.Distance(a, b);

		return distance < deadZone;
	}

	public static bool InReach(Color a, Color b)
	{
		float distance = Vector4.Distance(a, b);

		return distance < defaultDeadzone;
	}

	public static bool InReach(Color a, Color b, float deadZone)
	{
		float distance = Vector4.Distance(a, b);

		return distance < deadZone;
	}
	
	public static bool InReach(float a, float b)
	{
		float distance = b - a;

		return distance > 0 && distance < defaultDeadzone ||
		       distance < 0 && distance > -defaultDeadzone;
	}
	
	public static bool InReach(float a, float b, float deadZone)
	{
		float distance = b - a;

		return distance > 0 && distance < deadZone ||
		       distance < 0 && distance > -deadZone;
	}

    public static bool OutOfReach(Vector3 a, Vector3 b)
    {
	    float distance = Vector3.Distance(a, b);

	    return distance > defaultDeadzone;
    }
	
	public static bool OutOfReach(Vector3 a, Vector3 b, float deadZone)
	{
		float distance = Vector3.Distance(a, b);

		return distance > deadZone;
	}

	public static bool OutOfReach(Vector4 a, Vector4 b)
	{
		float distance = Vector4.Distance(a, b);

		return distance > defaultDeadzone;
	}

	public static bool OutOfReach(Vector4 a, Vector4 b, float deadZone)
	{
		float distance = Vector4.Distance(a, b);

		return distance > deadZone;
	}
	
	public static bool OutOfReach(float a, float b)
	{
		float distance = b - a;

		return distance > 0 && distance > defaultDeadzone ||
		       distance < 0 && distance < -defaultDeadzone;
	}
	
	public static bool OutOfReach(float a, float b, float deadZone)
	{
		float distance = b - a;

		return distance > 0 && distance > deadZone ||
		       distance < 0 && distance < -deadZone;
	}
}