import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../model/User';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<User>;

  constructor(private http: HttpClient, private storageService : StorageService) {
      this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
      this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): any {
      return this.currentUserSubject.value;
  }

  login(username: string, password: string):Observable<User> {

    let data = { username:username, password:password }
    let uri = `${environment.baseUrl}Authorization/login`;
    console.log(uri);
    console.log(JSON.stringify(data));
    let appUser =  this.http.post<User>(uri, data)
          .pipe(map(user => {
             debugger;
             console.log(`Respondio el login ${user}`);
              // store user details and jwt token in local storage to keep user logged in between page refreshes
              this.storageService.saveOnStorage('currentUser', user);
              this.currentUserSubject.next(user);
              return user;
          }));
    return appUser;
  }

  logout() {
      // remove user from local storage to log user out
      this.storageService.clearStorage();
      this.currentUserSubject.next(null);
  }

   checkIsAdmindUser(){
    const currentUser = this.currentUserValue;
    let rolesString = currentUser.rols;
    const isAdmind = rolesString.includes('Admin');
    return isAdmind;
  }

  getToken(): string {
    console.log("Entro al token");
    const currentUser = this.storageService.getFromStorage('currentUser');
    console.log(currentUser);
    if (currentUser) {
      console.log('retorna token');
      return currentUser.token ? currentUser.token : '';
    }
    console.log('No retorna token');
    return '';
  }

  getCurentUser(): User {
    const currentUser = this.storageService.getFromStorage('currentUser');
    console.log(currentUser);
    if (currentUser) {
      return currentUser;
    }
    return null;
  }
}