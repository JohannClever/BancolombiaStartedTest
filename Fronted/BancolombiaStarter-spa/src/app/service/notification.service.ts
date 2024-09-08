
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, timer } from 'rxjs';
import { INotification } from '../model/inotification';

@Injectable()
export class NotificationService {
  private notifications: INotification[] = [];
  timeOut: number = 5000;
  showNotificacionSource: BehaviorSubject<INotification[]> = new BehaviorSubject<INotification[]>(this.notifications);
  constructor() {
  }
  getNotification(): Observable<INotification[]> {
    return this.showNotificacionSource.asObservable();
  }

  Error(msg: string, summary?: string) {
    this.show('danger', summary, msg, this.timeOut);
  }

  Success(msg: string, summary?: string) {
    this.show('success', summary, msg, this.timeOut );
  }

  Info(msg: string, summary?: string) {
    this.show('info', summary, msg, this.timeOut);
  }

  Warn(msg: string, summary?: string) {
    this.show('warning', summary, msg, this.timeOut);
  }

  private show(severity: string, summary: string, msg: string, autoCloseTime?: number) {
    const notificacion: INotification = {
      severity: severity,
      summary: summary,
      detail: msg
    };
    this.notifications.push(notificacion);
    this.notify();
    if (autoCloseTime && autoCloseTime > 0) {
      this.removeNotificationAfterDelay(notificacion, autoCloseTime);
    }
  }

  private notify(): void {
    this.showNotificacionSource.next(this.notifications);
  }

  private removeNotificationAfterDelay(notification: INotification, delayTime: number): void {
    timer(delayTime).pipe().subscribe(() => {
      this.removeNotification(notification);
    });
  }

  private removeNotification(notification: INotification): void {
    const index = this.notifications.indexOf(notification);
    if (index !== -1) {
      this.notifications.splice(index, 1);
      this.notify();
    }
  }
}