using UnityEngine;
using UnityEngine.UI;
using Opsive.ThirdPersonController.UI;

namespace Opsive.ThirdPersonController.Demos.Clean
{
    /// <summary>
    /// Manages the various Third Person Controller clean scene demos.
    /// </summary>
    public class MyGameManager : MonoBehaviour
    {
        public enum Genre { Shooter, Adventure, RPG, Platformer, Pseudo3D, TopDown, PointClick, Last }
       
        [Tooltip("A reference to the adventure demo character")]
        [SerializeField] private GameObject m_AdventureCharacter;
       
       
//        [Tooltip("A reference to Blitz")]
//        [SerializeField] private GameObject m_Blitz;
//        
        [Tooltip("A reference to the crosshairs GameObject")]
        [SerializeField] private GameObject m_Crosshairs;
        
        [Tooltip("A reference to the adventure weapon wheel")]
        [SerializeField] private GameObject m_AdventureWeaponWheel;
        
        [Tooltip("A reference to the item pickup GameObject")]
        [SerializeField] private Transform m_ItemPickups;
//        [Tooltip("A reference to the pool barrier")]
//        [SerializeField] private GameObject m_PoolBarrier;

        public Genre CurrentGenre { get { return m_CurrentGenre; } }

        // Internal variables
		private Genre m_CurrentGenre = Genre.Adventure;
        private bool[] m_HasLoaded = new bool[(int)Genre.Last];
        private Vector3 m_BlitzPosition;
        private Quaternion m_BlitzRotation;

        // Component references
        private GameObject m_Character;
        private RigidbodyCharacterController m_CharacterController;
        private Inventory m_CharacterInventory;
        private AnimatorMonitor m_CharacterAnimatorMonitor;
        private CameraController m_CameraController;
        private CameraHandler m_CameraHandler;
        private GameObject m_WeaponWheel;
        private RigidbodyCharacterController m_BlitzCharacterController;
        
        /// <summary>
        /// Cache the component references.
        /// </summary>
        private void Awake()
        {
            m_CameraController = Camera.main.GetComponent<CameraController>();
            if (m_CameraController == null) {
                Debug.LogError("Error: Unable to find the CameraController.");
                enabled = false;
            }
            m_CameraHandler = m_CameraController.GetComponent<CameraHandler>();

//            m_BlitzCharacterController = m_Blitz.GetComponent<RigidbodyCharacterController>();
//            m_BlitzPosition = m_Blitz.transform.position;
//            m_BlitzRotation = m_Blitz.transform.rotation;

//            EventHandler.RegisterEvent<bool>(m_Blitz, "OnAllowGameplayInput", OnBlitzAllowGameplayInput);
        }

        /// <summary>
        /// Set the default values.
        /// </summary>
        private void Start()
        {
            
            m_HasLoaded[(int)m_CurrentGenre] = true;
            m_WeaponWheel = m_AdventureWeaponWheel;
			//m_AdventureWeaponWheel.SetActive(false);
            SwitchCharacters();
//           m_PoolBarrier.SetActive(false);
        }

        /// <summary>
        /// A new genre has been selected. Switch characters.
        /// </summary>
        private void SwitchCharacters()
        {
            GameObject character = null;
            switch (m_CurrentGenre) {
                
                case Genre.Adventure:
                    character = m_AdventureCharacter;
                    break;
               
                
            }
            character.SetActive(true);

            // Toggle the scheduler enable state by disabling and enabling it.
            var scheduler = GameObject.FindObjectOfType<Scheduler>();
            scheduler.enabled = false;
            scheduler.enabled = true;
	

            // Cache the character components.
            m_Character = character;
            m_CharacterController = character.GetComponent<RigidbodyCharacterController>();
            m_CharacterInventory = character.GetComponent<Inventory>();
            m_CharacterAnimatorMonitor = character.GetComponent<AnimatorMonitor>();
            m_CameraController.Character = character;
            m_CameraController.Anchor = character.transform;
            m_CameraController.DeathAnchor = character.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);
            m_CameraController.FadeTransform = character.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Chest);
        }

        /// <summary>
        /// Switch characters when the enter key is pressed.  
        /// </summary>
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return)) {
                SwitchGenres(true);
            }
        }

        /// <summary>
        /// Switch to the previous or next genre.
        /// </summary>
        /// <param name="next">Switch to the next genre?</param>
        public void SwitchGenres(bool next)
        {
            m_CharacterController.TryStopAllAbilities(true);
            m_Character.SetActive(false);
            m_CurrentGenre = (Genre)(((int)m_CurrentGenre + (next ? 1 : -1)) % (int)Genre.Last);
            if ((int)m_CurrentGenre < 0) m_CurrentGenre = Genre.PointClick;
           
            m_WeaponWheel.SetActive(false);
            SwitchCharacters();
            GenreSwitched();
            m_HasLoaded[(int)m_CurrentGenre] = true;
        }

        /// <summary>
        /// The genre has been switched. Update the various object references.
        /// </summary>
        private void GenreSwitched()
        {
            switch (m_CurrentGenre) {
                
			case Genre.Adventure:
				m_WeaponWheel = m_AdventureWeaponWheel;
				m_CameraHandler.ZoomStateName = "Zoom";
				m_CameraHandler.ZoomStateName = "StaticHeight";
				m_CameraController.ChangeState("Adventure", true);
				m_Crosshairs.SetActive(true);
                break;
                
                case Genre.Pseudo3D:
                    m_CameraHandler.ZoomStateName = string.Empty;
                    m_CameraController.ChangeState("ThirdPerson", false);
                    m_CameraController.ChangeState("Pseudo3D", true);
                    m_Crosshairs.SetActive(false);
                   
                    break;
                
            }
            m_WeaponWheel.SetActive(true);
            for (int i = 0; i < m_ItemPickups.childCount; ++i) {
                m_ItemPickups.GetChild(i).gameObject.SetActive(true);
            }

            // Start fresh.
            if (m_HasLoaded[(int)m_CurrentGenre]) {
                m_CharacterInventory.RemoveAllItems(false);
                m_CharacterInventory.LoadDefaultLoadout();
            }
            m_CharacterAnimatorMonitor.PlayDefaultStates();
            Respawn();

            // Update Blitz.
            var enableBlitz = m_CurrentGenre == Genre.Shooter || m_CurrentGenre == Genre.Adventure || m_CurrentGenre == Genre.Platformer || m_CurrentGenre == Genre.RPG;
            if (enableBlitz) {
                m_BlitzCharacterController.TryStopAllAbilities();
                m_BlitzCharacterController.SetPosition(m_BlitzPosition);
                m_BlitzCharacterController.SetRotation(m_BlitzRotation);
            }
//            m_Blitz.SetActive(enableBlitz);
        }

        /// <summary>
        /// Use a demo SpawnSelection component to respawn.
        /// </summary>
        private void Respawn()
        {
            var spawnLocation = CleanSpawnSelection.GetSpawnLocation();
            m_CharacterController.SetPosition(spawnLocation.position);
            m_CharacterController.SetRotation(spawnLocation.rotation);
            if (m_CurrentGenre == Genre.Shooter || m_CurrentGenre == Genre.Adventure || m_CurrentGenre == Genre.Platformer || m_CurrentGenre == Genre.RPG) {
                m_CameraController.ImmediatePosition();
            } else if (m_CurrentGenre == Genre.TopDown || m_CurrentGenre == Genre.PointClick) {
                m_CameraController.ImmediatePosition(Quaternion.Euler(53.2f, 0, 0));
            } else {
                m_CameraController.ImmediatePosition(Quaternion.Euler(26.56505f, 0, 0));
            }
        }

        /// <summary>
        /// The barrier should be active whenever Blitz is in control.
        /// </summary>
        private void OnBlitzAllowGameplayInput(bool allow)
        {
//            m_PoolBarrier.SetActive(allow);
        }
    }
}