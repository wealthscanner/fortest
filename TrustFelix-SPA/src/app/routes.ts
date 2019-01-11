import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MyAssetComponent } from './my-asset/my-asset.component';
import { AllAssetComponent } from './all-asset/all-asset.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes =  [
    { path: '', component: HomeComponent },
    {
        path: '', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'allasset', component: AllAssetComponent },   /* members */
            { path: 'myasset', component: MyAssetComponent },
            { path: 'messages', component: MessagesComponent }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
