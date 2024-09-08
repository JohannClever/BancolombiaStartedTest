import { Component, Input, OnInit } from '@angular/core';
import { ModalMessage } from '../../model/modalMessage';
import { ModalService } from 'src/app/service/modal.service';
import { modalTypeEnum } from 'src/app/model/modalType';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent implements OnInit {
  modalMessage: ModalMessage;
  buttons: any[] = [];
  centerButtons = false;
  showSecundaryButton = true;
  showModal = false;


  constructor(
    private mensajeModalService: ModalService
    ) {
    this.centerButtons = false;
    this.modalMessage = new ModalMessage();

   }

   cerrar(): void {
  }

  ngOnInit() {
    this.mensajeModalService.modalWindow.subscribe((modal:ModalMessage) => {
      if (!modal) {
        this.modalMessage = new ModalMessage();
        this.showModal = false;
        this.buttons =[];
        return;
    }
    console.log('modal:' + modal);
    this.showModal = true;
    this.modalMessage = modal;
    this.buttons = modal.buttons;
    this.centerButtons = this.buttons.length ==1;
  });
  }

  removerModal() {
    this.modalMessage = new ModalMessage();
  }


}