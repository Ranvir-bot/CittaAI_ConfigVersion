export interface DiffItem {
  type: 'Added' | 'Removed' | 'Modified';
  path: string;
  oldValue?: any;
  newValue?: any;
}