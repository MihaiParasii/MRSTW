import { Component } from '@angular/core';
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-admin-sidebar',
    imports: [
        RouterLink
    ],
  templateUrl: './admin-sidebar.component.html',
  standalone: true,
  styleUrl: './admin-sidebar.component.css'
})
export class AdminSidebarComponent {

}
