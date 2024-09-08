import { Injectable } from '@angular/core';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class StorageService {  
  constructor(private router: Router){}


 

  saveOnStorage(key:string, data: any){
    localStorage.setItem(key, JSON.stringify(data));
  }


  getFromStorage(key:string): any {
    return JSON.parse(localStorage.getItem(key));
  }

   clearStorage(): void {
    localStorage.clear();
  }
}
