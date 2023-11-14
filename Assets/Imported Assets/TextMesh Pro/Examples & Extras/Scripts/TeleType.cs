using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

namespace TMPro.Examples
{
    
    public class TeleType : MonoBehaviour
    {


        //[Range(0, 100)]
        //public int RevealSpeed = 50;

        private string label01 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";
        private string label02 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";


        private TMP_Text m_textMeshPro;
        private string m_textString;
        public bool skippedbBool = false;
        public bool textRevealing = false;
        public bool skipText = false;
        public bool mouseReleased = false;

        [SerializeField] float timeBtwnWords;
        [SerializeField] float timeBtwnChars;


        int i = 0;

        void Awake()
        {
            //m_textMeshPro.text = label01;
            //m_textMeshPro.enableWordWrapping = true;
            //m_textMeshPro.alignment = TextAlignmentOptions.Top;

            m_textMeshPro = GetComponent<TMP_Text>();
            m_textMeshPro.maxVisibleCharacters = 0;
        }

        public IEnumerator TypeText()
        {
            m_textMeshPro.maxVisibleCharacters = 0;
            // Force and update of the mesh to get valid information.
            m_textMeshPro.ForceMeshUpdate();
            //m_textString = InterfaceManager.instance.currentDialogue;
            bool played = false;
            int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
            int counter = 0;
            int visibleCount = 0;

            while (!played)
            {

                textRevealing = true;
                if (skipText)
                {
                    visibleCount = totalVisibleCharacters;
                    counter = m_textString.Length;
                    skipText = false;
                }

                visibleCount = counter % (totalVisibleCharacters + 1);

                m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?


          
                counter += 1;

                if (counter == m_textString.Length + 1)
                {
                    played = true;
                    

                }

                yield return new WaitForSeconds(timeBtwnChars);


                
            }
            textRevealing = false;
            Debug.Log("Done revealing the text.");
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (textRevealing)
                {
                    skipText = true;
                }

                else
                {
                    //InterfaceManager.instance.StartTalking();
                }
            }
        }

    }
}