#pragma once

#ifdef DLTDLL_EXPORTS
#define DLTDLL_API	__declspec(dllexport) //�Լ� ȣ��
#else
#define DLTDLL_API	__declspec(dllimport)
#endif
