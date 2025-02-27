import { Component } from '@angular/core';
import {RouterLink} from '@angular/router';
import {TranslatePipe} from "@ngx-translate/core";

@Component({
  selector: 'app-register',
    imports: [
        RouterLink,
        TranslatePipe
    ],
  templateUrl: './register.component.html',
  standalone: true,
  styleUrl: './register.component.css'
})
export class RegisterComponent {

}
