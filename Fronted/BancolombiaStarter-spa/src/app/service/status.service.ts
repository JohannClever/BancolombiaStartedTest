import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { RestService } from './rest.service';
import { Observable } from 'rxjs';
import { Status } from '../model/Status';


@Injectable({ providedIn: 'root' })
export class StatusService {
    constructor(private restService: RestService) { }

    getAll():Observable<Status[]> {
        return this.restService.get<Status[]>(`${environment.baseUrl}Status/GetAllStatus`);
    }
}
