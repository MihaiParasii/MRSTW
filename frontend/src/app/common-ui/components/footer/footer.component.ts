import { Component } from '@angular/core';
import {TranslatePipe} from "@ngx-translate/core";

@Component({
  selector: 'app-footer',
    imports: [
        TranslatePipe
    ],
  templateUrl: './footer.component.html',
  standalone: true,
  styleUrl: './footer.component.css'
})
export class FooterComponent {

}
