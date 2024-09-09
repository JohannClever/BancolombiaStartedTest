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


projectInfoForSuggestionData: ProjectInfoData = {
  userName:  "",
  description: "",
  goal:0,
  pledged: 0,
  projectPicture: "",
  userPicture: "",
  projectName:  "",
};

  constructor(private projectService: ProjectService, 
    private router: Router, 
) {
      
     }

  projects: Project[] = [];
  currentProject: ProjectInfoData;
  isMyProject: boolean = false ;
  isAdmind = false;
  projectId: number = 0;
  userId: number = 0;

  ngOnInit(): void {
   this.getDataFromRouter();  
   this.getProjectsToSugget(); 
  }

   

  private getDataFromRouter() {
    if (!(history.state && history.state.projectInfo))   
      this.router.navigate(['/projects'] );

     this.projectInfoForSuggestionData = history.state.projectInfo;
     this.projectId =  history.state.id;
     this.userId = history.state.userId;
  }

getProjectsToSugget() {
    this.projectService.getProjectsToSugget(this.projectId ).subscribe({
      next: (result) => {
        if (result) {
          this.projects = result;
          console.log(result);
        }
      }
    });
  }

  getSuggestion() {
    let projectIds: number[] = this.projects.map(project => project.id);
    this.router.navigate(['/projects/suggestion/result'], {state: { 
      id: this.projectId , 
      projectIds:projectIds,
      projectInfo:this.projectInfoForSuggestionData,
      userId: this.userId
    }
       });
    }
  
}
