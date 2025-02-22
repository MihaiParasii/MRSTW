import { Component } from '@angular/core';
import {RouterLink} from '@angular/router';
import {NavbarComponent} from '../../common-ui/components/navbar/navbar.component';

@Component({
  selector: 'app-login',
  imports: [
    RouterLink,
    NavbarComponent
  ],
  templateUrl: './login.component.html',
  standalone: true,
  styleUrl: './login.component.css'
})
export class LoginComponent {

}
