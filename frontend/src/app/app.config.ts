import {ApplicationConfig, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {HttpClient, provideHttpClient, withInterceptorsFromDi} from '@angular/common/http';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';


export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");

}

// export const appConfig: ApplicationConfig = {
//   providers: [
//     provideZoneChangeDetection({eventCoalescing: true}),
//     importProvidersFrom([
//       TranslateModule.forRoot({
//         loader: {
//           provide: TranslateLoader,
//           useFactory: (createTranslateLoader),
//           deps: [HttpClient]
//         },
//         defaultLanguage: 'en'
//       })
//     ]),
//     provideRouter(routes),
//     provideHttpClient(withInterceptorsFromDi())
//   ]
// };
//


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({eventCoalescing: true}),
    importProvidersFrom([
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: (createTranslateLoader),
          deps: [HttpClient]
        },
        defaultLanguage: 'ro'
      })
    ]),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi())
  ],
};
