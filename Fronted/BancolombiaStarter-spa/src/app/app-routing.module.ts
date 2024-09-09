import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {CommentsListComponent} from './components/Comments/comments-list/comments-list.component';
import {ProjectListComponent} from './components/Project/project-list/project-list.component';
import {ProjectFormComponent} from './components/Project/project-form/project-form.component';
import {ReportFormComponent} from './components/Comments/report-form/report-form.component';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './helpers/auth.guard';
import { ProjectInfoComponent } from './components/Project/project-info/project-info.component';
import { ProjectSuggestionsComponent } from './components/Project/project-suggestions/project-suggestions.component';
import { ProjectSuggestionsResultComponent } from './components/Project/project-suggestions-result/project-suggestions-result.component';

const routes: Routes = [
  {
    path : 'projects',
    component : ProjectListComponent,
    canActivate: [AuthGuard]

  },
  {
    path : 'projects/form',
    component : ProjectFormComponent,
    canActivate: [AuthGuard]

  },
  {
    path : 'projects/info',
    component : ProjectInfoComponent,
    canActivate: [AuthGuard]

  }, 
  {
    path : 'projects/suggestion',
    component : ProjectSuggestionsComponent,
    canActivate: [AuthGuard]

  }, 
  {
    path : 'projects/suggestion/result',
    component : ProjectSuggestionsResultComponent,
    canActivate: [AuthGuard]

  }, 
  {
    path :'comments/list',
    component: CommentsListComponent,
    canActivate: [AuthGuard]
  }, 
  { path: 'login', component: LoginComponent },
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  }

  
  

];

@NgModule({
  imports: [   
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(routes,{
      useHash: true
    })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
