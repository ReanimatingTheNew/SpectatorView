using UnityEngine;
using System.IO;
using System;

namespace SpectatorView
{
    public class Calibration : SV_Singleton<Calibration>
    {
        [Tooltip("Enable this checkbox if your camera is mounted below or to the left of your camera.")]
        public bool RotateCalibration = false;
        private bool prevRotateCalibration;

        public Vector3 Translation { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Vector2 DSLR_fov { get; private set; }
        public Vector4 DSLR_distortion { get; private set; }
        public Vector4 DSLR_matrix { get; private set; }

        void Start()
        {
            prevRotateCalibration = RotateCalibration;

            Translation = Vector3.zero;
            Rotation = Quaternion.identity;
            DSLR_fov = Vector2.one * 60.0f;
            DSLR_distortion = Vector4.zero;
            DSLR_matrix = Vector4.zero;

            ReadCalibrationData();
        }

        void OnValidate()
        {
            if (prevRotateCalibration != RotateCalibration)
            {
                prevRotateCalibration = RotateCalibration;

                Vector3 euler = -1 * Rotation.eulerAngles;
                Rotation = Quaternion.Euler(euler);

                Translation *= -1;

                gameObject.transform.localPosition = Translation;
                gameObject.transform.localRotation = Rotation;
            }
        }

        private void ReadCalibrationData()
        {
            // hardcoded calibration data
            string[] calibrationData = {
                "RMS: 44.5421",
                "DSLR RMS: 0.203851",
                "HoloLens RMS: 0.34048",
                "Translation: -0.00211806, -0.0910024, 0.00956102",
                "Rotation: 0.999703, -0.0181576, -0.0162388, 0.0187252, 0.999194, 0.0355101, 0.0155809, -0.0358036, 0.999237",
                "DSLR_fov: 75.9442, 47.5184",
                "Holo_fov: 47.7189, 27.9294",
                "DSLR_distortion: -0.130447, 0.0715056, -0.00174504, -0.000724455, -0.00542177",
                "DSLR_camera_Matrix: 901.984, 899.587, 705.553, 378.074"
            };

            foreach (var line in calibrationData)
            {
                String[] tokens = line.Split(new string[] { "\n", ":", ",", " " }, StringSplitOptions.RemoveEmptyEntries);
                string entryType = tokens[0].Trim().ToLower();

                if (entryType == "translation")
                {
                    Translation = new Vector3((float)Convert.ToDouble(tokens[1]),
                        (float)Convert.ToDouble(tokens[2]),
                        (float)Convert.ToDouble(tokens[3]));
                    if (!RotateCalibration)
                    {
                        Translation *= -1;
                    }

                    // Convert from OpenCV space to Unity space.
                    Translation = new Vector3(Translation.x, Translation.y, -1 * Translation.z);

                    //Debug.Log("Loaded calibration translation: " + Translation.x + ", " + Translation.y + ", " + Translation.z);
                }

                else if (entryType == "rotation")
                {
                    Rotation = Quaternion.LookRotation(
                        // Third column as forward direction.
                        new Vector3(
                            (float)Convert.ToDouble(tokens[7]),
                            (float)Convert.ToDouble(tokens[8]),
                            (float)Convert.ToDouble(tokens[9])
                        ),
                        // Second column as up direction.
                        new Vector3(
                            (float)Convert.ToDouble(tokens[4]),
                            (float)Convert.ToDouble(tokens[5]),
                            (float)Convert.ToDouble(tokens[6])
                        )
                    );

                    Vector3 euler = Rotation.eulerAngles;
                    if (!RotateCalibration)
                    {
                        euler *= -1;
                    }

                    // Convert from OpenCV space to Unity space.
                    euler.y *= -1;
                    Rotation = Quaternion.Euler(euler);

                    //Debug.Log("Loaded calibration quaternion: " + Rotation.x + ", " + Rotation.y + ", " + Rotation.z + ", " + Rotation.w);
                }

                else if (entryType == "dslr_fov")
                {
                    DSLR_fov = new Vector2((float)Convert.ToDouble(tokens[1]),
                        (float)Convert.ToDouble(tokens[2]));

                    //Debug.Log("Loaded calibration fov: " + DSLR_fov.x + ", " + DSLR_fov.y);
                }

                else if (entryType == "dslr_distortion")
                {
                    DSLR_distortion = new Vector4((float)Convert.ToDouble(tokens[1]),
                        (float)Convert.ToDouble(tokens[2]),
                        (float)Convert.ToDouble(tokens[3]),
                        (float)Convert.ToDouble(tokens[4]));

                    //Debug.Log("Loaded calibration dslr distortion: " + DSLR_distortion.x + ", " + DSLR_distortion.y + ", " + DSLR_distortion.z + ", " + DSLR_distortion.w);
                }

                else if (entryType == "dslr_camera_matrix")
                {
                    DSLR_matrix = new Vector4((float)Convert.ToDouble(tokens[1]),
                        (float)Convert.ToDouble(tokens[2]),
                        (float)Convert.ToDouble(tokens[3]),
                        (float)Convert.ToDouble(tokens[4]));

                    //Debug.Log("Loaded calibration dslr matrix: " + DSLR_matrix.x + ", " + DSLR_matrix.y + ", " + DSLR_matrix.z + ", " + DSLR_matrix.w);
                }
            }
        }
    }
}
