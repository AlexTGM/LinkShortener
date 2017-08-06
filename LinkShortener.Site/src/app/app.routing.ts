import { Routes, RouterModule } from '@angular/router';
 
import { LoginComponent, LogoutComponent } from './account/index';
import { LinksComponent } from './links/links.component';
import { LinkShortenerComponent } from './link-shortener/link-shortener.component';
import { AuthGuard } from './_guards/index';
 
const appRoutes: Routes = [
    { path: 'logout', component: LogoutComponent },
    { path: 'login', component: LoginComponent },
    { path: 'links', component: LinksComponent, canActivate: [AuthGuard] },
    { path: '', component: LinkShortenerComponent },
 
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];
 
export const routing = RouterModule.forRoot(appRoutes);