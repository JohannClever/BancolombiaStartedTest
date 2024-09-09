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
  templateUrl: './project-suggestions.component.html',
  styleUrls: ['./project-suggestions.component.css']
})
export class ProjectSuggestionsComponent implements OnInit {
getSuggestion() {
throw new Error('Method not implemented.');
}
openFinanceCreate() {
throw new Error('Method not implemented.');
}


projectInfoForSuggestionData: ProjectInfoData = {
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

  projects: Project[] = [];
  user: User;
  currentProject: ProjectInfoData;
  isMyProject: boolean = false ;
  isAdmind = false;
  projectId: number = 0;

  ngOnInit(): void {
   this.getDataFromRouter();  
   this.getProjectsToSugget(); 
  }

   

  private getDataFromRouter() {
    if (!(history.state && history.state.projectInfo))   
      this.router.navigate(['/projects'] );

     this.projectInfoForSuggestionData = history.state.projectInfo;
     this.projectId =  history.state.id;
  }

getProjectsToSugget() {
    this.projectService.getProjectsToSugget(this.projectId ).subscribe({
      next: (result) => {
        if (result) {
          this.projects = result;
        }
      }
    });
  }

  Update(){
  }
  
}
