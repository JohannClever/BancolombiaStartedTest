import { Injectable } from '@angular/core';
import { Project, UdateProject } from '../model/Project';
import { CreateProject } from '../model/Project'
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RestService } from './rest.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  constructor(private restService:RestService) { }

  getAll():Observable<Project[]> {
    debugger;
    let uri = `${environment.baseUrl}Project/GetAllProjects`;
    return this.restService.get<Project[]>(uri).pipe();
  }
  
  getProjects(data:any):Observable<Project[]> {
    let uri = `${environment.baseUrl}Project/GetProjects`;
    return  this.restService.get<Project[]>(uri,data);
  }

  get(id: string){
    return this.restService.get(`${environment.baseUrl}Project/${id}`);
  }

  getProjectsToSugget(id: number):Observable<Project[]> {
    return this.restService.get(`${environment.baseUrl}Project/GetProjectSuggestions/${id}`);
  }

  save(project: FormData){
    return this.restService.post(`${environment.baseUrl}Project/PostProjects`, project);

  }

  delete(id: string) {
    return this.restService.delete(`$${environment.baseUrl}Project/DeleteProjects/${id}`);
  }

  update(updated: UdateProject) {
    return this.restService.put(`${environment.baseUrl}Project/PutProjects`, updated);
  }

  mapTo(data: any): Project {
    return {
      id: data.id,
      name: data.name,
      description:data.description,
      goal: data.goal,
      pledged: data.pledged,
      backersCount: data.backersCount,
      pictureUrl: data.PictureUrl,
      userId: data.UserId,
      userName: data.UserName,
      userPicture: data.UserPicture,
      financedDate: data.financedDate,  // El símbolo '?' indica que es opcional, equivalente a un DateTime?
      creationOn: data.creationOn
      // Asigna otras propiedades de data a las propiedades correspondientes de User
    };
  }

  




}
