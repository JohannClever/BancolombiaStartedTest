import { Component, OnInit } from '@angular/core';

import { ProjectService } from '../../../service/project.service';
import { Router } from '@angular/router';
import { Project } from 'src/app/model/Project';
import { ProjectFilter } from 'src/app/model/ProjectFilter';
@Component({
  selector: 'app-services-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
})
export class ProjectListComponent implements OnInit {
  showTextIndex: number = -1;
  projects: Project[] = [];
  isAdmind = false;
  constructor(
    private projectService: ProjectService,
    private router: Router
    ) { }

  ngOnInit() {
     this.getProjects();
  }

showProject(project:Project){
  this.router.navigate(['/projects/info'], {state:{project:project} });
}

handleSearch(searchData: any) {
  this.handleGetReportsByFilter(searchData);
}

private handleGetReportsByFilter(filter : ProjectFilter) {
  if(filter.searchWord){
    this.GetReportsByFilter(filter).subscribe({
      next: (result) => {
        if (result) {
          this.projects = result;
        }
      }
    });
  }
  else
    this.getProjects();
}

create(){
  this.router.navigate(['/projects/form'], {state: { 
    operation:"create" , id: 0
     } });
}

exit() {
  this.router.navigate(['/']);
  }

private GetReportsByFilter(filter : ProjectFilter) {
  return this.projectService.getProjects(filter);
}


  private getProjects() {
    this.projectService.getAll().subscribe({
      next: (result) => {
        if (result) {
          this.projects = result;
        }
      }
    });
  }
}
