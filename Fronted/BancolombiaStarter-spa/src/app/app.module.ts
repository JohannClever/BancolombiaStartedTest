import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './components/navegacion/nav.component';
import { ReportFormComponent } from './components/Comments/report-form/report-form.component';
import { CommentsListComponent } from './components/Comments/comments-list/comments-list.component';
import { ProjectListComponent } from './components/Project/project-list/project-list.component';
import { from } from 'rxjs';
import { GridComponent } from './components/grid/grid.component';
import { JwtInterceptor } from './helpers/jwt-interceptor.service';
import { LoginComponent } from './components/login/login.component';
import { ErrorInterceptor } from './helpers/error-interceptor.service';
import { SearchComponent } from './components/Comments/search/search.component';
import { ModalComponent } from './components/modal/modal.component';
import { ModalService } from './service/modal.service';
import { SearchProjectComponent } from './components/Project/search-project/search-project.component';
import { ProjectFormComponent } from './components/Project/project-form/project-form.component';
import { ProjectInfoComponent } from './components/Project/project-info/project-info.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    ReportFormComponent,
    CommentsListComponent,
    ProjectListComponent,
    GridComponent,
    LoginComponent,
    SearchComponent,
    ModalComponent,
    SearchProjectComponent,
    ProjectFormComponent,
    ProjectInfoComponent ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule 
  ],
  providers: [
    ModalService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
