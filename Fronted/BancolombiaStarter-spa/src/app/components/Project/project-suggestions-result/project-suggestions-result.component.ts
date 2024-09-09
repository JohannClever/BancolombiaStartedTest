import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Project, ProjectResponseIA, UdateProject } from '../../../model/Project';
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
import { OpenIaService } from 'src/app/service/open-ia.service';

@Component({
  selector: 'app-project-form',
  templateUrl: './project-suggestions-result.component.html',
  styleUrls: ['./project-suggestions-result.component.css']
})
export class ProjectSuggestionsResultComponent implements OnInit {


projectInfo: CreateProject = {
  name: '',
  description: '',
  goal: 0,
  picture: undefined
};

projectInfoForSuggestionData: ProjectInfoData = {
  userName:  "",
  description: "",
  goal:0,
  pledged: 0,
  projectPicture: "",
  userPicture: "",
  projectName:  ""
};

  constructor(private openIaService: OpenIaService,
    private projectService: ProjectService, 
    private router: Router, ) {
      
     }

  project: ProjectResponseIA ;
  currentProject: ProjectInfoData;
  isMyProject: boolean = false ;
  isAdmind = false;
  projectId: number = 0;
  userId: number = 0;

  ngOnInit(): void {
   this.getDataFromRouter(); 
  }

   

  private getDataFromRouter() {
    if (!(history.state && history.state.projectIds))   
      this.router.navigate(['/projects'] );

     this.projectInfoForSuggestionData = history.state.projectInfo;
     this.projectId =  history.state.id;
     this.userId = history.state.userId;

     let data = {
      suggestionProjectsId: history.state.projectIds, // Los projectIds que pasaste por el router
      projectId: this.projectId              // El projectId que también pasaste por el router
    };

     this.getProjectsToSugget(data) 
  }

getProjectsToSugget(data: any) {
    this.openIaService.getProjectsToSugget(data).subscribe({
      next: (result) => {
        if (result) {
          this.project = result;
          this.projectInfoForSuggestionData.description = this.project.description;
          this.projectInfoForSuggestionData.projectName = this.project.name;
          this.projectInfoForSuggestionData.goal = this.project.goal;
        }
      }
    });
  }

  update() {
      let updateData: UdateProject = {
        id: this.projectId,
        name: this.project.name,
        description: this.project.description,
        goal: this.project.goal
      };
      console.log(updateData);
      this.projectService.update(updateData).subscribe({
        next: (result) => {
          if (result) {
            console.log("Info Updated");
            let projectUpdated = {
              id: this.projectId,
              name: this.project.name,
              description: this.project.description,
              goal: this.project.goal,
              pictureUrl: this.projectInfoForSuggestionData.projectPicture,
              userId: this.userId
            };
            console.log(projectUpdated);
            this.router.navigate(['/projects/info'], 
              {
                state:
                {
                  project:projectUpdated
                } 
              });
          }
        }
      });
    }

    close() {
      this.router.navigate(['/projects'] );
      }
    
}
