import { Component } from '@angular/core';
import {RouterLink} from '@angular/router';
import {NavbarComponent} from '../../common-ui/components/navbar/navbar.component';
import {TranslatePipe} from '@ngx-translate/core';

@Component({
  selector: 'app-login',
  imports: [
    RouterLink,
    NavbarComponent,
    TranslatePipe
  ],
  templateUrl: './login.component.html',
  standalone: true,
  styleUrl: './login.component.css'
})
export class LoginComponent {

}
