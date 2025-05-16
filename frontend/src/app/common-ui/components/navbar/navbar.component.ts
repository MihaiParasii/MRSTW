import {Component, inject} from '@angular/core';
import {RouterLink} from '@angular/router';
import {NgForOf, NgIf, UpperCasePipe} from '@angular/common';
import {HomeComponent} from '../../../user-ui/components/home/home.component';
import {TranslatePipe, TranslateService} from '@ngx-translate/core';
import {ActionBarComponent} from '../action-bar/action-bar.component';

@Component({
  selector: 'app-navbar',
  imports: [
    RouterLink,
    HomeComponent,
    UpperCasePipe,
    NgForOf,
    NgIf,
    TranslatePipe,
    ActionBarComponent,
  ],
  templateUrl: './navbar.component.html',
  standalone: true,
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  private translateService: TranslateService = inject(TranslateService);
  protected currentLanguage: string = 'ro';
  protected languages: string[] = ['ro', 'ru', 'en'];


  public translateText(lang: string) {
    this.currentLanguage = lang;
    this.translateService.use(lang);
  }

  public IsLoggedIn(): boolean {
    return false;
  }
}
