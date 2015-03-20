; Global Media Hotkeys for SubGadget
; By Robert "Quantum" Cross
; Questions? quantumcross at gmail
; v2 - 12/19/2011
;------------------------------------------------------------------------------
; Default configuration is set to respond to standard media buttons on a most
; keyboards. Volume up, down, and mute respond to Ctrl+VolUp/Down/Mute buttons.
; You can change the hotkeys by changing the lines that end in ::
; A list of valid keys can be found at:
; http://www.autohotkey.com/docs/KeyList.htm
;
; Modifiers are as follows:
; # 	Win (Windows logo key)
; ! 	Alt
; ^ 	Control
; + 	Shift
; More information on modifiers can be found at:
; http://www.autohotkey.com/docs/Hotkeys.htm#Symbols
;
; Example: Ctrl+Alt+Left Arrow would be -
; ^!Left::
;------------------------------------------------------------------------------
; The first line makes sure that if SubGadget is already in focus, the script
; doesn't do anything, this avoids double triggers in case the gadget is in
; focus and one of your hotkeys contains a media key
#IfWinNotActive, SubGadget

;Play/Pause
Media_Play_Pause::
ControlFocus,, SubGadget
ControlSend,, {Media_Play_Pause}, SubGadget
return

;Previous Track
Media_Prev::
ControlFocus,, SubGadget
ControlSend,, {Media_Prev}, SubGadget
return

;Next Track
Media_Next::
ControlFocus,, SubGadget
ControlSend,, {Media_Next}, SubGadget
return

;Stop
Media_Stop::
ControlFocus,, SubGadget
ControlSend,, {Media_Stop}, SubGadget
return

;Volume Mute
^Volume_Mute::
ControlFocus,, SubGadget
ControlSend,, {Volume_Mute}, SubGadget
return

;Volume Up
^Volume_Up::
ControlFocus,, SubGadget
ControlSend,, {Volume_Up}, SubGadget
return

;Volume Down
^Volume_Down::
ControlFocus,, SubGadget
ControlSend,, {Volume_Down}, SubGadget
return
