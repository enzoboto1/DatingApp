import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class Register {
  protected model: any = {};

  @Output() cancelRegister = new EventEmitter<boolean>();

  cancel() {
    this.cancelRegister.emit(false);
  }

  register() {
    // TODO: wire up registration HTTP call
    this.cancelRegister.emit(false);
  }
}
