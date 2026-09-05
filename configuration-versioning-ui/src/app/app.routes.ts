import { Routes } from '@angular/router';
import { Editor } from './components/editor/editor';
import { Diff } from './components/diff/diff';
import { History } from './components/history/history';

export const routes: Routes = [

 {
    path: '',
    component: Editor
  },
  {
    path: 'history',
    component: History
  },
  {
    path: 'diff',
    component: Diff
  }

];
