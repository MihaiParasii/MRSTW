import {Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {SidebarCategoriesComponent} from '../sidebar-categories/sidebar-categories.component';

@Component({
  selector: 'app-home',
  imports: [
    RouterOutlet,
    SidebarCategoriesComponent,
  ],
  templateUrl: './home.component.html',
  standalone: true,
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
