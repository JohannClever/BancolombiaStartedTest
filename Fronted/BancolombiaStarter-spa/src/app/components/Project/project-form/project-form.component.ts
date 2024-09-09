import { Component, OnInit } from '@angular/core';
import { Project, UdateProject } from '../../../model/Project';
import { CreateProject } from '../../../model/Project';

import {ProjectService} from '../../../service/project.service'
import { User } from 'src/app/model/User';
import { Status } from 'src/app/model/Status';
import { Router } from '@angular/router';
import { ModalMessage } from 'src/app/model/modalMessage';
import { ModalService } from 'src/app/service/modal.service';

@Component({
  selector: 'app-project-form',
  templateUrl: './project-form.component.html',
  styleUrls: ['./project-form.component.css']
})
export class ProjectFormComponent implements OnInit {

  projectInfo: CreateProject = {
    name: '',
    description: '',
    goal: 0,
    picture: undefined
};

  constructor(private projectService: ProjectService, 
    private router: Router, 
    private modalService: ModalService) {


     }

  
  users: User[];
  project: Project;
  statuses: Status[];

  selectedService: any = null;
  selectedStatus: any = null;
  selectedUser: any = "";
  

  isEdit:boolean = false;

  registerId: number = 0;
  inputDescription: string = '';

  isAdmind = false;

  ngOnInit(): void {
    this.isEdit = false;
    this.getDataFromRouter();
  }

  private getDataFromRouter() {
    console.log(history);
    if (!(history.state && history.state.operation)) {   
      this.router.navigate(['/projects'] );
    }

    if(history.state.operation =="edit" && history.state.id)
    {
        this.isEdit = true;
        this.registerId = history.state.id;
        this.projectInfo = history.state.project;
        this.project = history.state.project;
     }
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
        this.projectInfo.picture = file;
    }
}

 save(){
    console.log("entro a guardar");
    console.log(this.registerId);
    if (this.ValidateInputs()) {
      if (this.registerId > 0) {
        this.handlePut();
      }
      else {
        this.handlePost(); 
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

cancel() {
    this.router.navigate(['/projects']);
}

  
  handleConfirmation(){
    this.modalService.hideModalWindow();
  }

  private handlePut() {
    let updateData: UdateProject = {
      id: this.registerId,
      name: this.projectInfo.name,
      description: this.projectInfo.description,
      goal: this.projectInfo.goal
    };

    this.PutProject(updateData).subscribe({
      next: (result) => {
        if (result) {
          let projectUpdated = {
            id: this.registerId,
            name: this.projectInfo.name,
            description: this.projectInfo.description,
            goal: this.projectInfo.goal,
            pictureUrl: this.project.pictureUrl
          };
          this.router.navigate(['/projects/info'], {state:{project:projectUpdated} });
        }
      }
    });
  }
    
  private PutProject(data : any) {
   return this.projectService.update(data);
  }

  private handlePost() {
    console.log();
    const formData = new FormData();
    formData.append('Name', this.projectInfo.name);
    formData.append('Description', this.projectInfo.description);
    formData.append('Goal', this.projectInfo.goal.toString());
  
    // Solo añade el archivo si se ha seleccionado uno
    if (this.projectInfo.picture) {
      formData.append('Picture', this.projectInfo.picture, this.projectInfo.picture.name);
    }

    formData.forEach((value, key) => {
      console.log(`${key}: ${value}`);
    });
    return this.projectService.save(formData).subscribe({
      next: (result) => {
        if (result) {
          this.router.navigate(['/projects'] );
        }
      }
    });
  } 

  private ValidateInputs(): boolean {

    return true;
  }

}
