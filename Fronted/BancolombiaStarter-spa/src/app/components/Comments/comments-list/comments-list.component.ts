import { Component, OnInit } from '@angular/core';

import { ReportService} from '../../../service/report.service'
import { Router } from '@angular/router';
import { ReportFilter } from '../../../model/ReportFilter';
import { ProjectService } from 'src/app/service/project.service';
import { UserService } from 'src/app/service/user.service';
import { StatusService } from 'src/app/service/status.service';
import { User } from 'src/app/model/User';
import { Project } from 'src/app/model/Project';
import { Status } from 'src/app/model/Status';
import { ModalMessage } from 'src/app/model/modalMessage';
import { ModalService } from '../../../service/modal.service';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comments-list.component.html',
  styleUrls: ['./comments-list.component.css']
})
export class CommentsListComponent implements OnInit {
  isAdmind = false;
  comments: any = [];
  comment: any;
  service:Project;
  deleteReport:any;

  customHeaders: { [key: string]: string } = {};
  hideColumns = ['id', 'companyServiceid','idUser'];
  actions = [
    { Icon: 'fas fa-edit', Action: this.edit.bind(this) },
    { Icon: 'fas fa-trash', Action: this.delete.bind(this) },
  ];

  users: User[];
  services: Project[]= [];
  statuses: Status[];

  constructor(
    private reportService: ReportService,
    private projectService: ProjectService,
    private userService: UserService,
    private statusService: StatusService,
    private router: Router,
    private modalService:ModalService
    ) { }

  ngOnInit() {
    this.getDataFromRouter();
    this.getDataFromServer();
    this.getReports();
    this.getCustomHeader();

  }

  getCustomHeader() {
    this.customHeaders =  {
      companyServiceName: 'Servicio',
      statusName: 'Estado',
      userName: 'Nombre de usuario',
      observation: 'Observacion',
      creationOn: 'Fecha de creación'
    };
    console.log('pre send '+this.customHeaders);
  }


  private getDataFromRouter() {
    
    if (!this.isAdmind && !(history.state && history.state.service)) {
      this.router.navigate(['/services']);
    }

    if (history.state  && history.state.service  &&!this.isAdmind) {
      this.service = this.projectService.mapTo(history.state.service)
      this.services.push(this.service);
    }
  }

  private getDataFromServer() {

    if (this.isAdmind) {
      this.projectService.getAll().subscribe({
        next: (result) => {
          if (result) {
            this.services = result;
          }
        }
      });
    }

    this.userService.getAll().subscribe({
      next: (result) => {
        if (result) {
          this.users = result;
        }
      }
    });

    this.statusService.getAll().subscribe({
      next: (result) => {
        if (result) {
          this.statuses = result;
        }
      }
    });
  }

  private getReports() {
    console.log(this.comments);
    const reportFilter: ReportFilter = {
      serviceId: this.service ? this.service.id : 0,
      statusId: 0,
      idUser: 'none',
    }; 
    this.comments = this.reportService.getAll(reportFilter).subscribe({
      next: (result) => {
        if (result) {
          this.comments = result;
        }
      }
    });
    console.log(this.comments);
  }



edit(row: any) {
  console.log(row);
  this.router.navigate(['/report/form'], {state: { 
    operation:"edit",
    companyServices: this.services ,
    statuses: this.statuses, 
    users: this.users,
    report:row
     } });

}

delete(row: any) {
  this.deleteReport = row;
  let buttonsActions =  [
    { name: 'Aceptar', action: this.handleDeleteFromConfirmation.bind(this) },
    { name: 'Cancelar', action: this.closeModal.bind(this) },
  ];

  let message: ModalMessage = {
    message: "¿Desea eliminar el reporte?",
    buttons:  buttonsActions
  }

  this.modalService.showModalWindow(message);
}


handleDeleteFromConfirmation(){
  let deleteItem = {
    id: this.deleteReport.id
  }
  this.handleDeleteReport(deleteItem);
  this.modalService.hideModalWindow();
}

closeModal(){
  this.deleteReport = null;
  this.modalService.hideModalWindow();
}

abrirModal(row: any) {
let data = {
  id: row.id
};
const title = 'Confirmación';
const message = '¿Estás seguro de que deseas realizar esta acción?';


//this.modalService.confirm(title, message).subscribe((result) => {
//  if (result) {
 //   this.handleDeleteReport(data);
 // }
//});
}

create(){
  this.router.navigate(['/report/form'], {state: { 
    operation:"create",
    companyServices: this.services ,
    statuses: this.statuses, 
    users: this.users
     } });
}

exit() {
  this.router.navigate(['/']);
  }



private handleGetReports() {
this.comments = this.getReports();
}



private handleDeleteReport(data : any) {
this.DeleteReport(data).subscribe({
  next: (result) => {
    if (result) {
      this.handleGetReports();
    }
  }
});
}

private DeleteReport(data : any) {
  console.log(data);
return this.reportService.delete(data);
}

handleSearch(searchData: any) {
  this.handleGetReportsByFilter(searchData);
}

private handleGetReportsByFilter(filter : string) {
  this.GetReportsByFilter(filter).subscribe({
    next: (result) => {
      if (result) {
        this.comments = result;
      }
      else{
        this.handleGetReports();
      }
    }
  });
}

private GetReportsByFilter(filter : string) {
  return this.reportService.getAll(filter);
}

}
