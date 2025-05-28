using YogaApp.Core.Enums;

namespace YogaApp.Application.Helpers;

public static class BMICalculator
{
    public static double CalculateBMI(double weight, double heightCm)
    {
        var bmi = weight / Math.Pow(heightCm / 100.0, 2);
        
        return bmi;
    }
    
    public static BMIStatus CalculateBmiStatus(double bmi)
    {
        if (bmi < 18.5) return BMIStatus.Underweight;
        if (bmi < 25) return BMIStatus.Healthy;
        if (bmi < 30) return BMIStatus.Overweight;
        return BMIStatus.Obese;
    }
}