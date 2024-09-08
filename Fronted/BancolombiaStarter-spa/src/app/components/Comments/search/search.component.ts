import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Project } from 'src/app/model/Project';
import { Status } from 'src/app/model/Status';
import { User } from 'src/app/model/User';
import { ModalComponent } from '../../modal/modal.component';
import { ModalService } from 'src/app/service/modal.service';
import { ModalMessage } from 'src/app/model/modalMessage';
import { modalTypeEnum } from 'src/app/model/modalType';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  @Input() services: Project[];
  @Input() statuses: Status[];
  @Input() users: User[];

  selectedService: Project;
  selectedStatus: Status;
  selectedUser: User;

  
  @Output() searchEvent = new EventEmitter<any>();
  constructor() { }

  ngOnInit() {
    console.log(this.services);
  }


  search() {
    const searchData = {
      serviceId: this.selectedService?this.selectedService:0,
      statusId: this.selectedStatus?this.selectedStatus:0 ,
      idUser: this.selectedUser?this.selectedUser:"none",
    };
    this.searchEvent.emit(searchData);
  }

  clean(){
    this.selectedService = null;
    this.selectedStatus = null;
    this.selectedUser = null;
  }


}
