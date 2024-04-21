#pragma once

#ifdef DLTDLL_EXPORTS
#define DLTDLL_API	__declspec(dllexport) //함수 호출
#else
#define DLTDLL_API	__declspec(dllimport)
#endif
