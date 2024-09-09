import { Injectable } from '@angular/core';
import { Project, ProjectResponseIA } from '../model/Project';
import { CreateProject } from '../model/Project'
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RestService } from './rest.service';

@Injectable({
  providedIn: 'root'
})
export class OpenIaService {
  constructor(private restService:RestService) { }

  getProjectsToSugget(input:any):Observable<ProjectResponseIA> {
    return this.restService.post(`${environment.baseUrl}api/OpenAi/generate-suggestions`,input);
  }


  mapTo(data: any): ProjectResponseIA {
    return {
      name: data.name,
      description:data.description,
      goal: data.goal,
      creationOn: data.creationOn
      // Asigna otras propiedades de data a las propiedades correspondientes de User
    };
  }

  




}
