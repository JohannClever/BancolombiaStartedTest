import { Injectable } from '@angular/core';
import { User } from '../model/User';
import { environment } from 'src/environments/environment';
import { RestService } from './rest.service';
import { Observable } from 'rxjs';


@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private restService: RestService) { }

    getAll():Observable<User[]> {
        return this.restService.get<User[]>(`${environment.baseUrl}User/GetUsers`);
    }

    getUserById(id:string):Observable<User> {
        return this.restService.get<User>(`${environment.baseUrl}User/GetUsers/${id}`);
    }
}
