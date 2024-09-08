import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-project-form',
  templateUrl: './project-info.component.html',
  styleUrls: ['./project-info.component.css']
})
export class ProjectInfoComponent implements OnInit {
openFinanceCreate() {
throw new Error('Method not implemented.');
}



  constructor(private projectService: ProjectService, 
    private userService:UserService,
    private authenticationService: AuthenticationService,
    private router: Router, 
    private modalService: ModalService) {
      console.log("inicio");
     }

  
  user: User;
  project: Project;

  isAdmind = false;

  ngOnInit(): void {
   debugger;
   this.getDataFromRouter();

    console.log("va a consultar usuario");
    this.userService.getUserByName(this.project.userId).subscribe({
      next: (result) => {
        console.log(result);
        
        if (result) {
          this.user = result;
        }
      }
    });
    
 
    
  }

  private getDataFromRouter() {
    if (!(history.state && history.state.project)) {   
      this.router.navigate(['/projects'] );
    }
        this.project = history.state.project;

  }


}
