import { Injectable } from '@angular/core';
import { getDiff } from 'json-difference';

import { DiffItem } from '../models/diff-item.model';

@Injectable({
  providedIn: 'root'
})
export class DiffService {

  getDiffItems(oldJson: string, newJson: string): DiffItem[] {

    const oldData = JSON.parse(oldJson);
    const newData = JSON.parse(newJson);

    const diff = getDiff(oldData, newData);

    const items: DiffItem[] = [];

    for (const item of diff.added) {
      items.push({
        type: 'Added',
        path: item[0],
        newValue: item[1]
      });
    }

    for (const item of diff.removed) {
      items.push({
        type: 'Removed',
        path: item[0],
        oldValue: item[1]
      });
    }

    for (const item of diff.edited) {
      items.push({
        type: 'Modified',
        path: item[0],
        oldValue: item[1],
        newValue: item[2]
      });
    }

    return items;
  }
}