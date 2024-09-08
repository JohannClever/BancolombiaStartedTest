import { EventEmitter, Injectable, Output } from '@angular/core';
import { ModalMessage } from '../model/modalMessage';

@Injectable()
export class ModalService {
@Output() modalWindow: EventEmitter<ModalMessage> = new EventEmitter();

constructor() { }

showModalWindow(modalMessage: ModalMessage) {
    this.modalWindow.emit(modalMessage);
}

hideModalWindow() {
    this.modalWindow.emit(null);
 }
} 
