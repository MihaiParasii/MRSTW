import {Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {NavbarComponent} from '../../common-ui/components/navbar/navbar.component';

@Component({
  selector: 'app-auth-layout',
  imports: [
    RouterOutlet,
    NavbarComponent
  ],
  templateUrl: './auth-layout.component.html',
  standalone: true,
  styleUrl: './auth-layout.component.css'
})
export class AuthLayoutComponent {
}
