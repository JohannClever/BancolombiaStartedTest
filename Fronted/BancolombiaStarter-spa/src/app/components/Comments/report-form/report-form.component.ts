import { Component, OnInit } from '@angular/core';
import { Report, UdateReport } from '../../../model/Report';
import { CreateReport } from '../../../model/Report';

import {ReportService} from '../../../service/report.service'
import { User } from 'src/app/model/User';
import { Project } from 'src/app/model/Project';
import { Status } from 'src/app/model/Status';
import { Router } from '@angular/router';
import { ModalMessage } from 'src/app/model/modalMessage';
import { ModalService } from 'src/app/service/modal.service';

@Component({
  selector: 'app-report-form',
  templateUrl: './report-form.component.html',
  styleUrls: ['./report-form.component.css']
})
export class ReportFormComponent implements OnInit {


  constructor(private reportService: ReportService, 
    private router: Router, 
    private modalService: ModalService) { }

  users: User[];
  services: Project[]= [];
  statuses: Status[];

  selectedService: any = null;
  selectedStatus: any = null;
  selectedUser: any = "";
  

  isService:boolean;
  isStatus:boolean;
  isUser:boolean;
  isObservation:boolean;

  registerId: number = 0;
  inputObservation: string = '';

  isAdmind = false;

  ngOnInit(): void {

    this.getDataFromRouter();
    if(!this.isAdmind ){
      this.selectedService = this.services[0].id;
      this.isService = true;
    }
  }

  private getDataFromRouter() {
    if (!(history.state && history.state.companyServices)) {
      
      if(this.isAdmind ){
        this.router.navigate(['/report/list']);
      }
      else
      {
        this.router.navigate(['/services'] );
      }
    }

    if (history.state  && history.state.companyServices) {
      this.services = history.state.companyServices;
      this.statuses = history.state.statuses;
      this.users = history.state.users;

      if(history.state.operation =="edit")
      {
        this.registerId = history.state.report.id;
        this.inputObservation =  history.state.report.observations;
        this.selectedService =  history.state.report.companyServiceId;
        this.selectedStatus = history.state.report.statusId;
        this.selectedUser = history.state.report.idUser;
      }
    }
  }

  save(){
    if (this.ValidateInputs()) {
      if (this.registerId > 0) {
        let updateData: UdateReport = {
          id: this.registerId,
          companyServiceId: this.selectedService,
          statusId: this.selectedStatus,
          observations: this.inputObservation,
        };
        this.handlePutReport(updateData);
      }
      else {
        let  data: CreateReport = {
          companyServiceId: this.selectedService,
          statusId: this.selectedStatus,
          idUser: this.selectedUser,
          observations: this.inputObservation
        };
        this.handlePostReport(data); 
      }


    }
    else{
      let buttonsActions =  [
        { name: 'Aceptar', action: this.handleConfirmation.bind(this) },
      ];
    
      let message: ModalMessage = {
        message: "Error en los datos verifiquelos por favor",
        buttons:  buttonsActions
      }
    
      this.modalService.showModalWindow(message);
    }

  }

  returnToPreviusPage()
  {
    if(this.isAdmind ){
      this.router.navigate(['/report/list']);
    }
    else
    {
      this.router.navigate(['/report/list'], {state:{service:this.services[0]}});
    }
  }

  cancel() {
      this.router.navigate(['/report/list'], {state:{service:this.services[0]}});
  }


  
  handleConfirmation(){
    this.modalService.hideModalWindow();
  }

  private handlePutReport(data : any) {
    this.PutReport(data).subscribe({
      next: (result) => {
        if (result) {
          this.returnToPreviusPage();
        }
      }
    });
  }
    
  private PutReport(data : any) {
   return this.reportService.update(data);
  }

  private handlePostReport(data : any) {
    this.PostReport(data).subscribe({
      next: (result) => {
        if (result) {
          this.returnToPreviusPage();
        }
      }
    });
  }

  private PostReport(data : any) {
    return this.reportService.save(data);
  }

  private ValidateInputs(): boolean {
    this.isService = this.selectedService === null;
    this.isStatus = this.selectedStatus === null;
    this.isUser =  this.isAdmind?  this.selectedUser === '': false;
    this.isObservation = this.inputObservation === '';
    return !this.isService && !this.isStatus && !this.isUser && !this.isObservation;
  }

}
