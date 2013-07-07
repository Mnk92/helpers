#include "windows.h"
int WINAPI WinMain(HINSTANCE hInst, HINSTANCE hPrevInstm, LPSTR lpCmdLine, int nShowCmd){
	if(lpCmdLine!=NULL && *lpCmdLine!=NULL){
		STARTUPINFO si;
		ZeroMemory(&si, sizeof(si));
		si.cb = sizeof(si);
		
		PROCESS_INFORMATION pi;
		ZeroMemory(&pi, sizeof(pi));
		
		if(!CreateProcess(NULL, lpCmdLine, NULL, NULL, FALSE, CREATE_NO_WINDOW, NULL, NULL, &si, &pi))return 0;
		__try{
			WaitForSingleObject(pi.hProcess, INFINITE);

			DWORD exitCode;
			return GetExitCodeProcess(pi.hProcess, &exitCode);
		}
		__finally{
			CloseHandle( pi.hProcess );
			CloseHandle( pi.hThread );
		}
	}
	MessageBox(NULL, "Please specify program path as first command line argument.", NULL, MB_OK|MB_ICONERROR);
	return -1;
}
