import { Injectable } from '@angular/core';
import { Report, UdateReport } from '../model/Report';
import { CreateReport } from '../model/Report';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RestService } from './rest.service';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  constructor(private restService:RestService) { }



  getAll(data:any):Observable<Report[]> {
    let uri = `${environment.baseUrl}Report/GetResports`;
    return  this.restService.get<Report[]>(uri,data);
  }
  
  save(report: CreateReport){
    console.log(report);  
    return this.restService.post(`${environment.baseUrl}Report/PostReport`, report);

  }

  delete(id: any) {
    return this.restService.delete(`${environment.baseUrl}Report/DeleteReport`,id);
  }

  update( updatedreport: UdateReport) {
    return this.restService.put(`${environment.baseUrl}Report/PutReport`, updatedreport);

  }

  




}
