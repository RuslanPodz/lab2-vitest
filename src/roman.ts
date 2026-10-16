export function toRoman(num: number): string {
  if (!Number.isInteger(num)) {
    throw new Error('NOT_INTEGER');
  }
  if (num < 1 || num > 3999) {
    throw new Error('OUT_OF_RANGE');
  }

  const values = [1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1];
  const letters = [
    'M',
    'CM',
    'D',
    'CD',
    'C',
    'XC',
    'L',
    'XL',
    'X',
    'IX',
    'V',
    'IV',
    'I',
  ];

  let result = '';
  let rest = num;

  for (let i = 0; i < values.length; i++) {
    while (rest >= values[i]) {
      result = result + letters[i];
      rest = rest - values[i];
    }
  }

  return result;
}
