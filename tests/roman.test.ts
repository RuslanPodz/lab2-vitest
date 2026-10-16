import { describe, it, expect } from 'vitest';
import { toRoman } from '../src/roman';

describe('toRoman', () => {
  it('повертає XII для числа 12', () => {
    const number = 12;

    const result = toRoman(number);

    expect(result).toBe('XII');
  });

  it('повертає MCMXCIV для числа 1994', () => {
    expect(toRoman(1994)).toBe('MCMXCIV');
  });

  it('використовує віднімання для 4, 9, 40 і 90', () => {
    expect(toRoman(4)).toBe('IV');
    expect(toRoman(9)).toBe('IX');
    expect(toRoman(40)).toBe('XL');
    expect(toRoman(90)).toBe('XC');
  });

  it('повертає I для найменшого допустимого числа 1', () => {
    expect(toRoman(1)).toBe('I');
  });

  it('повертає MMMCMXCIX для найбільшого допустимого числа 3999', () => {
    expect(toRoman(3999)).toBe('MMMCMXCIX');
  });

  it('кидає помилку для нуля', () => {
    expect(() => toRoman(0)).toThrow('OUT_OF_RANGE');
  });

  it('кидає помилку для числа 4000', () => {
    expect(() => toRoman(4000)).toThrow('OUT_OF_RANGE');
  });

  it('кидає помилку для від’ємного числа', () => {
    expect(() => toRoman(-5)).toThrow('OUT_OF_RANGE');
  });

  it('кидає помилку для дробового числа', () => {
    expect(() => toRoman(3.5)).toThrow('NOT_INTEGER');
  });
});
