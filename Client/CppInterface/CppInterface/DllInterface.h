#pragma once
#include<math.h>
#include<string.h>
#include<iostream>
#define _DllExport _declspec(dllexport)

#define UnityLog(acStr)  char acLogStr[512] = { 0 }; sprintf_s(acLogStr, "%s",acStr); Debug::Log(acLogStr,strlen(acStr));


extern "C"
{ 
	//C++ Call C#
	class Debug
	{
	public:
		static void (*Log)(char* message,int iSize);
	};




	// C# call C++
	void _DllExport InitCSharpDelegate(void (*Log)(char* message, int iSize));

	float _DllExport GetDistance(float x1, float y1, float x2, float y2);
}

