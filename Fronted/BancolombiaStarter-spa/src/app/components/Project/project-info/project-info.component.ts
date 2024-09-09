import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Project, UdateProject } from '../../../model/Project';
import { CreateProject } from '../../../model/Project';

import {ProjectService} from '../../../service/project.service'
import { User } from 'src/app/model/User';
import { Status } from 'src/app/model/Status';
import { Router } from '@angular/router';
import { ModalMessage } from 'src/app/model/modalMessage';
import { ModalService } from 'src/app/service/modal.service';
import { StorageService } from 'src/app/service/storage.service';
import { UserService } from '../../../service/user.service';
import { AuthenticationService } from 'src/app/service/authentication.service';
import { ProjectInfoData } from 'src/app/model/ProjectInfo';

@Component({
  selector: 'app-project-form',
  templateUrl: './project-info.component.html',
  styleUrls: ['./project-info.component.css']
})
export class ProjectInfoComponent implements OnInit {
openFinanceCreate() {
throw new Error('Method not implemented.');
}


projectInfoData: ProjectInfoData = {
  userName:  "",
  description: "",
  goal:0,
  pledged: 0,
  projectPicture: "",
  userPicture: "",
  projectName:  ""
};

  constructor(private projectService: ProjectService, 
    private userService:UserService,
    private authenticationService: AuthenticationService,
    private router: Router, 
    private cdr: ChangeDetectorRef,
    private modalService: ModalService) {
      
     }

  
  user: User;
  project: Project;
  isMyProject: boolean = false ;
  isAdmind = false;

  ngOnInit(): void {
   this.getDataFromRouter();   
   this.getProyectUserInformation();
   this.checkIsMyProject();
  }

  private getProyectUserInformation(){
    this.userService.getUserById(this.project.userId).subscribe({
      next: (result) => {
        console.log(result);
        
        if (result) {
          this.user = result;
          this.projectInfoData.userPicture = this.user.pictureUrl;
          this.projectInfoData.userName = this.user.userName;
        }
      }
    });
  }

  private checkIsMyProject(){
    let currentUser  = this.authenticationService.getCurentUser();
    this.isMyProject = currentUser.id == this.project.userId;
    this.cdr.detectChanges();  // Obligamos a Angular a detectar cambios
  }
   

  private getDataFromRouter() {
    if (!(history.state && history.state.project))   
      this.router.navigate(['/projects'] );
    
     this.project = history.state.project;

     this.projectInfoData.projectPicture = this.project.pictureUrl;
     this.projectInfoData.description = this.project.description;
     this.projectInfoData.pledged = this.project.pledged;
     this.projectInfoData.goal = this.project.goal;
     this.projectInfoData.projectName = this.project.name;
  }

  update(){
    this.router.navigate(['/projects/form'], {state: { 
      operation:"edit" , id: this.project.id , project: this.project
       } });
  }

  sugeestion(){
    this.router.navigate(['/projects/suggestion'], {state: { 
       id: this.project.id , projectInfo:this.projectInfoData
       } });
   
  }
  
}
